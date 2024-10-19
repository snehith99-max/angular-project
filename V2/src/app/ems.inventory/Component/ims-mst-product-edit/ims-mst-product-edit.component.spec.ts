import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsMstProductEditComponent } from './ims-mst-product-edit.component';

describe('ImsMstProductEditComponent', () => {
  let component: ImsMstProductEditComponent;
  let fixture: ComponentFixture<ImsMstProductEditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsMstProductEditComponent]
    });
    fixture = TestBed.createComponent(ImsMstProductEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
