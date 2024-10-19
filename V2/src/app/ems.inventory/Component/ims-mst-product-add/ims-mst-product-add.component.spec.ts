import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsMstProductAddComponent } from './ims-mst-product-add.component';

describe('ImsMstProductAddComponent', () => {
  let component: ImsMstProductAddComponent;
  let fixture: ComponentFixture<ImsMstProductAddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsMstProductAddComponent]
    });
    fixture = TestBed.createComponent(ImsMstProductAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
