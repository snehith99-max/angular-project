import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SbcMstDynamicdbexceptionerrorviewComponent } from './sbc-mst-dynamicdbexceptionerrorview.component';

describe('SbcMstDynamicdbexceptionerrorviewComponent', () => {
  let component: SbcMstDynamicdbexceptionerrorviewComponent;
  let fixture: ComponentFixture<SbcMstDynamicdbexceptionerrorviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SbcMstDynamicdbexceptionerrorviewComponent]
    });
    fixture = TestBed.createComponent(SbcMstDynamicdbexceptionerrorviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
