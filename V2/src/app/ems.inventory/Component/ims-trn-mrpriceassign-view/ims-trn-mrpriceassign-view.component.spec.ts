import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnMrpriceassignViewComponent } from './ims-trn-mrpriceassign-view.component';

describe('ImsTrnMrpriceassignViewComponent', () => {
  let component: ImsTrnMrpriceassignViewComponent;
  let fixture: ComponentFixture<ImsTrnMrpriceassignViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnMrpriceassignViewComponent]
    });
    fixture = TestBed.createComponent(ImsTrnMrpriceassignViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
