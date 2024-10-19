import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnMaterialindentViewComponent } from './ims-trn-materialindent-view.component';

describe('ImsTrnMaterialindentViewComponent', () => {
  let component: ImsTrnMaterialindentViewComponent;
  let fixture: ComponentFixture<ImsTrnMaterialindentViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnMaterialindentViewComponent]
    });
    fixture = TestBed.createComponent(ImsTrnMaterialindentViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
