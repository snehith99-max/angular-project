import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LawMstInstituteviewComponent } from './law-mst-instituteview.component';

describe('LawMstInstituteviewComponent', () => {
  let component: LawMstInstituteviewComponent;
  let fixture: ComponentFixture<LawMstInstituteviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LawMstInstituteviewComponent]
    });
    fixture = TestBed.createComponent(LawMstInstituteviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
