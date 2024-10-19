import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnSalesreturnViewComponent } from './ims-trn-salesreturn-view.component';

describe('ImsTrnSalesreturnViewComponent', () => {
  let component: ImsTrnSalesreturnViewComponent;
  let fixture: ComponentFixture<ImsTrnSalesreturnViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnSalesreturnViewComponent]
    });
    fixture = TestBed.createComponent(ImsTrnSalesreturnViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
