import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsMstProductViewComponent } from './ims-mst-product-view.component';

describe('ImsMstProductViewComponent', () => {
  let component: ImsMstProductViewComponent;
  let fixture: ComponentFixture<ImsMstProductViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsMstProductViewComponent]
    });
    fixture = TestBed.createComponent(ImsMstProductViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
